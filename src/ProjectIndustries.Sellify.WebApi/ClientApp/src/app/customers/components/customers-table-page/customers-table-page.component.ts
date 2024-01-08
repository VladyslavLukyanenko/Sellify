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
import {CustomerRowData, OrderRowData, OrderStatus, PurchasedProductData} from "../../../sellify-api";
import {BehaviorSubject} from "rxjs";
import {ColumnDef} from "../../../shared/components/expandable-table/expandable-table.component";
import {LayoutService} from "../../../core/services/layout.service";
import {TemplatePortal} from "@angular/cdk/portal";
import {DateUtil} from "../../../core/services/date.util";
import {CustomersDataSource} from "../../services/customers.data-source";
import * as moment from "moment";
import {ToolbarService} from "../../../core/services/toolbar.service";
import {debounceTime, distinctUntilChanged} from "rxjs/operators";
import {
  GeneralPeriodTypes,
  isStartAtValid,
  monthFormat,
  startAtDefaults
} from "../../../shared/components/period-picker/period-picker.component";
import {ActivatedRoute, Router} from "@angular/router";
import {PurchasedOrdersDataSource} from "../../services/purchased-orders.data-source";

const searchTermMinLen = 3;

@Component({
  selector: "app-customers-table-page",
  templateUrl: "./customers-table-page.component.html",
  styleUrls: ["./customers-table-page.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: "Page"
  },
  providers: [
    CustomersDataSource,
    PurchasedOrdersDataSource
  ],
  encapsulation: ViewEncapsulation.None
})
export class CustomersTablePageComponent extends DisposableComponentBase implements OnInit {
  @ViewChild("createOrderTmpl", {static: true}) createOrderTmpl: TemplateRef<any>;
  @ViewChild("toolbarSearchTmpl", {static: true}) toolbarSearchTmpl: TemplateRef<any>;
  @ViewChild("actionsColTmpl", {static: true}) actionsColTmpl: TemplateRef<any>;

  period$ = new BehaviorSubject<GeneralPeriodTypes>(GeneralPeriodTypes.AllTime);
  startAt$ = new BehaviorSubject<number | string>(startAtDefaults[this.period$.value]);

  columns: ColumnDef<CustomerRowData>[];
  private searchTermPublisher$ = new BehaviorSubject<string>(null);

  constructor(private layoutService: LayoutService,
              private viewContainerRef: ViewContainerRef,
              readonly purchasedOrdersDataSource: PurchasedOrdersDataSource,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              readonly customersDataSource: CustomersDataSource,
              private toolbarService: ToolbarService) {
    super();
    toolbarService.setTitle("Customers");
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

        if (period && !(period in GeneralPeriodTypes) || !isStartAtValid(period, startAt)) {
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

        this.customersDataSource.setSearch(s);
        this.customersDataSource.setPeriod(startTimeAt, endTimeAt);
        await this.customersDataSource.refreshData();
      });

    this.columns = [
      {
        title: "Customer name",
        value: r => r.firstName && (r.firstName + " " + r.lastName) ||  r.email,
        classes: {
          headerCol: "CustomersTable-nameCol",
          dataCol: ""
        },
      },
      {
        title: "Amount spent",
        value: r => "$" + r.totalSpent + " spent",
        classes: {
          headerCol: "CustomersTable-spentCol",
          dataCol: ""
        },
      },
      {
        title: "Total orders",
        value: r => r.totalOrdersCount + " orders",
        classes: {
          headerCol: "CustomersTable-totalOrdersCol",
          dataCol: ""
        },
      },
      {
        title: "Last purchase",
        value: r => DateUtil.humanizeDate(r.lastPurchasedAt),
        classes: {
          headerCol: "CustomersTable-lastPurchaseAtCol",
          dataCol: ""
        },
      },
      {
        title: "Actions",
        value: this.actionsColTmpl,
        classes: {
          headerCol: "CustomersTable-actionsCol",
          dataCol: ""
        },
      }
    ];
  }

  async toggleDetails(shouldFetch: boolean, c: CustomerRowData): Promise<void> {
    if (shouldFetch) {
      this.purchasedOrdersDataSource.customerId$.next(c.id);
      await this.purchasedOrdersDataSource.refreshData();
    } else {
      this.purchasedOrdersDataSource.customerId$.next(null);
    }
  }

  async pushSearchToRoute(s: string): Promise<void> {
    if (s?.trim().length < searchTermMinLen) {
      s = null;
    }

    this.searchTermPublisher$.next(s);
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

interface CustomerOrderInfoData {
  productTitle: string;
  purchasedAt: string;
  price: number;
  status: OrderStatus;
}
