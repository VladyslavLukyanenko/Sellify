import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  TemplateRef,
  ViewChild,
  ViewContainerRef,
  ViewEncapsulation
} from "@angular/core";
import {DisposableComponentBase} from "../../../shared/components/disposable.component-base";
import {LayoutService} from "../../../core/services/layout.service";
import {TemplatePortal} from "@angular/cdk/portal";
import {MenuItem} from "primeng/api";
import {OrdersDataSource} from "../../services/orders.data-source";
import {OrderRowData, OrdersService, OrderStatus} from "../../../sellify-api";
import {BehaviorSubject} from "rxjs";
import {ColumnDef} from "../../../shared/components/expandable-table/expandable-table.component";
import {DateUtil} from "../../../core/services/date.util";
import {ToolbarService} from "../../../core/services/toolbar.service";
import {KeyValuePair} from "../../../core/models/key-value-pair.model";
import {ActivatedRoute, Router} from "@angular/router";
import {debounceTime, distinctUntilChanged} from "rxjs/operators";
import {
  GeneralPeriodTypes,
  isStartAtValid, monthFormat,
  startAtDefaults
} from "../../../shared/components/period-picker/period-picker.component";
import * as moment from "moment";


const supportedOrderStatuses: KeyValuePair<string, OrderStatus>[] = Object.keys(OrderStatus)
  .filter(s => OrderStatus.hasOwnProperty(s))
  .map(s => ({
    key: s,
    value: OrderStatus[s]
  }));

const orderStatusesFilter = supportedOrderStatuses.slice();
orderStatusesFilter.unshift({
  key: "All",
  value: null
});

const searchTermMinLen = 3;

@Component({
  selector: "app-orders-table-page",
  templateUrl: "./orders-table-page.component.html",
  styleUrls: ["./orders-table-page.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: "Page"
  },
  providers: [
    OrdersDataSource
  ],
  encapsulation: ViewEncapsulation.None
})
export class OrdersTablePageComponent extends DisposableComponentBase implements OnInit {
  @ViewChild("createOrderTmpl", {static: true}) createOrderTmpl: TemplateRef<any>;
  @ViewChild("toolbarSearchTmpl", {static: true}) toolbarSearchTmpl: TemplateRef<any>;
  @ViewChild("statusColTmpl", {static: true}) statusColTmpl: TemplateRef<any>;

  supportedOrderStatuses = supportedOrderStatuses;
  orderStatusesFilter = orderStatusesFilter;
  orderStatus = OrderStatus;

  details$ = new BehaviorSubject<OrderDetailsData>(null);
  columns: ColumnDef<OrderRowData>[];

  period$ = new BehaviorSubject<GeneralPeriodTypes>(GeneralPeriodTypes.AllTime);
  startAt$ = new BehaviorSubject<number | string>(startAtDefaults[this.period$.value]);
  private searchTermPublisher$ = new BehaviorSubject<string>(null);

  constructor(private layoutService: LayoutService,
              private viewContainerRef: ViewContainerRef,
              readonly ordersDataSource: OrdersDataSource,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              private ordersService: OrdersService,
              private toolbarService: ToolbarService) {
    super();
    toolbarService.setTitle("Orders");
  }

  trackById = (_: number, r: OrderRowData) => r?.id;

  ngOnInit(): void {
    this.layoutService
      .displayOnSidebar(new TemplatePortal(this.createOrderTmpl, this.viewContainerRef), this.destroy$);
    this.layoutService
      .displayOnToolbar(new TemplatePortal(this.toolbarSearchTmpl, this.viewContainerRef), this.destroy$);

    this.pushSearchToRoute(this.activatedRoute.snapshot.queryParams.q?.trim());
    this.searchTermPublisher$
      .pipe(
        this.untilDestroy(),
        distinctUntilChanged(),
        debounceTime(500)
      )
      .subscribe(s => this.mergeQueryParams({q: s?.trim() || null}));

    this.activatedRoute.queryParams
      .pipe(
        this.untilDestroy()
      )
      .subscribe(async params => {
        const s = params.q;
        const period = params.period;
        const startAt = period === GeneralPeriodTypes.Yearly ? +params.startAt : params.startAt;
        const status = params.status;

        if (period && !(period in GeneralPeriodTypes) || !isStartAtValid(period, startAt)
          || status && !(status in OrderStatus)) {
          await this.router.navigate([], {
            relativeTo: this.activatedRoute,
            queryParams: {period: GeneralPeriodTypes.AllTime},
            replaceUrl: true
          });
          return;
        }

        this.startAt$.next(startAt);
        this.period$.next(period);

        // todo: refactor it. this is duplicate
        let startTimeAt: string = null;
        let endTimeAt: string = null;
        if (period === GeneralPeriodTypes.Monthly) {
          const start = moment(startAt, monthFormat).startOf("month");
          startTimeAt = start.toISOString();
          endTimeAt = start.clone().endOf("month").toISOString();
        } else if (period === GeneralPeriodTypes.Yearly) {
          const start = moment([startAt, 1, 1]).endOf("year");
          startTimeAt = start.clone().startOf("year").toISOString();
          endTimeAt = start.toISOString();
        }

        this.ordersDataSource.setSearch(s);
        this.ordersDataSource.setStatus(status);
        this.ordersDataSource.setPeriod(startTimeAt, endTimeAt);
        await this.ordersDataSource.refreshData();
      });

    this.columns = [
      {
        title: "Order",
        value: r => r.id,
        classes: {
          headerCol: "OrdersTable-idCol",
          dataCol: ""
        },
      },
      {
        title: "Date",
        value: r => DateUtil.humanizeDateTime(r.createdAt),
        classes: {
          headerCol: "OrdersTable-dateCol",
          dataCol: ""
        },
      },
      {
        title: "Customer",
        value: r => r.customerName || r.customerEmail || r.invoiceEmail,
        classes: {
          headerCol: "OrdersTable-customerCol",
          dataCol: ""
        },
      },
      {
        title: "Product",
        value: r => r.purchasedProduct.title,
        classes: {
          headerCol: "OrdersTable-productCol",
          dataCol: ""
        },
      },
      {
        title: "Total",
        value: r => "$" + r.totalPrice.toFixed(2),
        classes: {
          headerCol: "OrdersTable-totalCol",
          dataCol: ""
        },
      },
      {
        title: "Status",
        value: this.statusColTmpl,
        classes: {
          headerCol: "OrdersTable-statusCol",
          dataCol: ""
        },
      }
    ];
  }

  toggleDetails(shouldFetch: boolean, order: OrderRowData): Promise<void> {
    if (shouldFetch) {
      this.details$.next({
        paidAt: order.createdAt,
        // purchasedCountry: "United States",
        totalPrice: order.totalPrice,
        products: [
          {quantity: order.purchasedProduct.quantity, productTitle: order.purchasedProduct.title}
        ]
      });
    } else {
      this.details$.next(null);
    }

    return Promise.resolve();
  }

  async changeStatus(nextStatus: OrderStatus, row: OrderRowData): Promise<void> {
    await this.asyncTracker.executeAsAsync(
      this.ordersService.ordersChangeStatus(row.id, nextStatus)
    );

    await this.ordersDataSource.refreshData();
  }

  async pushSearchToRoute(s: string): Promise<void> {
    if (s?.trim().length < searchTermMinLen) {
      s = null;
    }

    this.searchTermPublisher$.next(s);
  }

  async pushStatusToRoute(status?: OrderStatus): Promise<void> {
    await this.mergeQueryParams({status});
  }

  private async mergeQueryParams(queryParams: any): Promise<void> {
    await this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams,
      queryParamsHandling: "merge"
    });
  }

  async pushStartAt(startAt: number | string): Promise<void> {
    await this.mergeQueryParams({
      startAt
    });
  }

  async pushPeriod(period: GeneralPeriodTypes): Promise<void> {
    await this.mergeQueryParams({
      period,
      startAt: startAtDefaults[period]
    });
  }
}

interface OrderTransactionDetailsData {
  productTitle: string;
  quantity: number;
}

interface OrderDetailsData {
  products: OrderTransactionDetailsData[];
  paidAt: string;
  purchasedCountry?: string;
  totalPrice: number;
}
