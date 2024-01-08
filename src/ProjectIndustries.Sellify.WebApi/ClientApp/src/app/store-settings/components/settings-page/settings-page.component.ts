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
import {ColumnDef} from "../../../shared/components/expandable-table/expandable-table.component";
import {AnalyticsService, SaveBindingCommand, WebhookRowData, WebhooksService} from "../../../sellify-api";
import {LayoutService} from "../../../core/services/layout.service";
import {TemplatePortal} from "@angular/cdk/portal";
import {WebhooksDataSource} from "../../services/webhooks.data-source";
import {DateUtil} from "../../../core/services/date.util";
import {BehaviorSubject} from "rxjs";
import {debounceTime, filter, map} from "rxjs/operators";
import {WebhookReceiverType} from "../../models/webhook-receiver-type.model";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {FormUtil} from "../../../core/services/form.util";
import {KeyValuePair} from "../../../core/models/key-value-pair.model";
import {ConfirmationService} from "primeng/api";
import {ActivatedRoute, Router} from "@angular/router";
import {ToolbarService} from "../../../core/services/toolbar.service";

@Component({
  selector: "app-settings-page",
  templateUrl: "./settings-page.component.html",
  styleUrls: ["./settings-page.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    WebhooksDataSource
  ],
  host: {
    class: "Page"
  },
  encapsulation: ViewEncapsulation.None
})
export class SettingsPageComponent extends DisposableComponentBase implements OnInit {
  @ViewChild("createOrderTmpl", {static: true}) createOrderTmpl: TemplateRef<any>;

  @ViewChild("toolbarSearchTmpl", {static: true}) toolbarSearchTmpl: TemplateRef<any>;
  @ViewChild("actionsColTmpl", {static: true}) actionsColTmpl: TemplateRef<any>;

  columns: ColumnDef<WebhookRowData>[];

  events$ = new BehaviorSubject<KeyValuePair[]>([]);
  receiverTypes = Object.keys(WebhookReceiverType)
    .filter(k => WebhookReceiverType.hasOwnProperty(k))
    .map(k => ({
      key: k,
      value: k
    }));

  isAddWebhookDialogVisible$ = new BehaviorSubject(false);
  isRemoveDialogVisible$ = new BehaviorSubject(false);

  form = new SaveWebhookBindingFormGroup();

  removeRow: WebhookRowData;

  constructor(private layoutService: LayoutService,
              private viewContainerRef: ViewContainerRef,
              private webhooksService: WebhooksService,
              private confirmService: ConfirmationService,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              readonly webhooksDataSource: WebhooksDataSource,
              private toolbarService: ToolbarService) {
    super();
    toolbarService.setTitle("Store Settings");
  }

  trackById = (_: number, r: WebhookRowData) => r?.id;

  async ngOnInit(): Promise<void> {
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
        this.webhooksDataSource.setSearch(s);
      });

    this.columns = [
      {
        title: "Site",
        value: r => r.receiverType,
        classes: {
          headerCol: "StoreSettingsTable-siteCol"
        },
      },
      {
        title: "Date added",
        value: r => DateUtil.humanizeDateTime(r.createdAt),
        classes: {
          headerCol: "StoreSettingsTable-createdAtCol"
        },
      },
      {
        title: "Webhook event",
        value: r => r.eventType,
        classes: {
          headerCol: "StoreSettingsTable-eventTypeCol"
        },
      },
      {
        title: "Actions",
        value: this.actionsColTmpl,
        classes: {
          headerCol: "StoreSettingsTable-actionsCol"
        },
      }
    ];

    this.webhooksDataSource.refreshData();
    const eventTypes = await this.asyncTracker.executeAsAsync(
      this.webhooksService.webhooksGetSupportedWebhooks().pipe(map(_ => _.payload.map(r => ({
        key: r,
        value: r
      }))))
    );

    this.events$.next(eventTypes);

    this.isAddWebhookDialogVisible$
      .pipe(
        this.untilDestroy(),
        filter(r => !r)
      )
      .subscribe(() => this.form.reset());

    this.isRemoveDialogVisible$
      .pipe(
        this.untilDestroy(),
        filter(r => !r)
      )
      .subscribe(() => this.removeRow = null);
  }


  async saveBinding(): Promise<void> {
    if (this.form.invalid) {
      FormUtil.validateAllFormFields(this.form);
      return;
    }

    const data = this.form.value;
    try {
      const op = data.id
        ? this.webhooksService.webhooksUpdate(data.id, data)
        : this.webhooksService.webhooksCreate(data);
      await this.asyncTracker.executeAsAsync(op);
      this.isAddWebhookDialogVisible$.next(false);
      await this.webhooksDataSource.refreshData();
    } catch (e) {

    }
  }

  edit(row: WebhookRowData): void {
    this.form.patchValue(row, {emitEvent: false});
    this.isAddWebhookDialogVisible$.next(true);
  }

  confirmRemove(row: WebhookRowData): void {
    this.removeRow = row;
    this.isRemoveDialogVisible$.next(true);
  }

  async remove(): Promise<void> {
    await this.asyncTracker.executeAsAsync(this.webhooksService.webhooksRemove(this.removeRow.id));
    await this.webhooksDataSource.refreshData();
    this.isRemoveDialogVisible$.next(false);
  }

  async pushSearchToRoute(s: string): Promise<void> {
    await this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: {q: s?.trim() || null},
      queryParamsHandling: "merge"
    });
  }
}


export class SaveWebhookBindingFormGroup extends FormGroup {
  constructor(data?: SaveBindingCommand) {
    super({
      id: new FormControl(),
      receiverType: new FormControl(data?.receiverType, [Validators.required]),
      eventType: new FormControl(data?.eventType, [Validators.required]),
      listenerEndpoint: new FormControl(data?.listenerEndpoint, [Validators.required]),
    });
  }
}
