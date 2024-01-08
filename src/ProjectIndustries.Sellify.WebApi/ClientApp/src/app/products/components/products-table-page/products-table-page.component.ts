import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  TemplateRef,
  ViewChild,
  ViewContainerRef,
  ViewEncapsulation
} from "@angular/core";
import {ProductsDataSource} from "../../services/products.data-source";
import {MenuItem} from "primeng/api";
import {ColumnDef} from "../../../shared/components/expandable-table/expandable-table.component";
import {AnalyticsService, ProductRowData, ProductsService} from "../../../sellify-api";
import {LayoutService} from "../../../core/services/layout.service";
import {TemplatePortal} from "@angular/cdk/portal";
import {DisposableComponentBase} from "../../../shared/components/disposable.component-base";
import {BehaviorSubject} from "rxjs";
import {DomSanitizer, SafeResourceUrl, SafeStyle} from "@angular/platform-browser";
import {NotificationService} from "../../../core/services/notifications/notification.service";
import {OperationStatusMessage} from "../../../core/services/notifications/messages.model";
import {debounceTime, filter} from "rxjs/operators";
import {ActivatedRoute, Router} from "@angular/router";
import {ToolbarService} from "../../../core/services/toolbar.service";

@Component({
  selector: "app-products-table-page",
  templateUrl: "./products-table-page.component.html",
  styleUrls: ["./products-table-page.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    ProductsDataSource
  ],
  host: {
    class: "Page"
  },
  encapsulation: ViewEncapsulation.None
})
export class ProductsTablePageComponent extends DisposableComponentBase implements OnInit {
  @ViewChild("createProductTmpl", {static: true}) createOrderTmpl: TemplateRef<any>;

  @ViewChild("toolbarSearchTmpl", {static: true}) toolbarSearchTmpl: TemplateRef<any>;
  @ViewChild("productTitleColTmpl", {static: true}) productTitleColTmpl: TemplateRef<any>;
  @ViewChild("categoriesColTmpl", {static: true}) categoriesColTmpl: TemplateRef<any>;
  @ViewChild("actionsColTmpl", {static: true}) actionsColTmpl: TemplateRef<any>;

  supportedSortOptions: MenuItem[] = [
    {
      label: "All time",
      command: () => alert("Not implemented")
    }
  ];

  columns: ColumnDef<ProductRowData>[];

  isEditorDialogVisible$ = new BehaviorSubject(false);
  isRemoveDialogVisible$ = new BehaviorSubject(false);

  selectedProductId: number = null;

  constructor(private layoutService: LayoutService,
              private viewContainerRef: ViewContainerRef,
              private domSanitizer: DomSanitizer,
              private productsService: ProductsService,
              private notificationService: NotificationService,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              readonly productsDataSource: ProductsDataSource,
              private toolbarService: ToolbarService) {
    super();
    toolbarService.setTitle("Products");
  }

  trackById = (_: number, r: ProductRowData) => r?.id;

  ngOnInit(): void {
    this.layoutService
      .displayOnSidebar(new TemplatePortal(this.createOrderTmpl, this.viewContainerRef), this.destroy$);
    this.layoutService
      .displayOnToolbar(new TemplatePortal(this.toolbarSearchTmpl, this.viewContainerRef), this.destroy$);

    this.activatedRoute.queryParams
      .pipe(
        this.untilDestroy(),
        debounceTime(300)
      )
      .subscribe(params => {
        const s = params.q;
        this.productsDataSource.setSearch(s);
      });

    this.isEditorDialogVisible$
      .pipe(
        this.untilDestroy(),
        filter(it => !it)
      )
      .subscribe(() => this.selectedProductId = null);

    this.columns = [
      {
        title: "Product",
        value: this.productTitleColTmpl,
        classes: {
          headerCol: "ProductsTable-titleCol",
          dataCol: "ProductsTable-titleDataCol"
        },
      },
      {
        title: "Category",
        value: this.categoriesColTmpl,
        classes: {
          headerCol: "ProductsTable-categoriesCol",
          dataCol: ""
        },
      },
      {
        title: "Stock",
        value: r => r.stock + " units left",
        classes: {
          headerCol: "ProductsTable-stockCol",
          dataCol: ""
        },
      },
      {
        title: "Price",
        value: r => "$" + r.price,
        classes: {
          headerCol: "ProductsTable-priceCol",
          dataCol: ""
        },
      },
      {
        title: "Actions",
        value: this.actionsColTmpl,
        classes: {
          headerCol: "ProductsTable-actionsCol",
          dataCol: ""
        },
      }
    ];
  }

  async refresh(): Promise<void> {
    this.isEditorDialogVisible$.next(false);
    await this.productsDataSource.refreshData();
  }

  sanitizePictureUrl(picture: string): SafeStyle {
    return this.domSanitizer.bypassSecurityTrustStyle(`url(${picture})`);
  }

  edit(row: ProductRowData): void {
    this.selectedProductId = row.id;
    this.isEditorDialogVisible$.next(true);
  }

  confirmRemoval(row: ProductRowData): void {
    this.selectedProductId = row.id;
    this.isRemoveDialogVisible$.next(true);
  }

  async remove(): Promise<void> {
    try {
      await this.asyncTracker.executeAsAsync(this.productsService.productsRemove(this.selectedProductId));
      this.notificationService.success(OperationStatusMessage.REMOVED);
      await this.productsDataSource.refreshData();
    } catch (e) {
      this.notificationService.success(OperationStatusMessage.FAILED);
    } finally {
      this.isRemoveDialogVisible$.next(false);
    }
  }

  async pushSearchToRoute(s: string): Promise<void> {
    await this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: {q: s?.trim() || null},
      queryParamsHandling: "merge"
    });
  }
}
