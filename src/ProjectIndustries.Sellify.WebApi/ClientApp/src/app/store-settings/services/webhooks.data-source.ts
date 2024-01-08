import {InfiniteScrollDataSourceBase} from "../../core/services/infinite-scroll.data-source-base";
import {WebhookRowData, WebhooksService} from "../../sellify-api";
import {Observable} from "rxjs/internal/Observable";
import {ApiContract} from "../../core/models/api-contract.model";
import {PagedList} from "../../core/models/paged-list.model";
import {Injectable} from "@angular/core";
import {BehaviorSubject, combineLatest} from "rxjs";
import {distinctUntilChanged} from "rxjs/operators";
import {CollectionViewer} from "@angular/cdk/collections";

@Injectable()
export class WebhooksDataSource extends InfiniteScrollDataSourceBase<WebhookRowData> {
  private _search$ = new BehaviorSubject<string>(null);

  search$ = this._search$.asObservable().pipe(distinctUntilChanged());

  constructor(private webhooksService: WebhooksService) {
    super();
  }

  connect(collectionViewer: CollectionViewer): Observable<WebhookRowData[]> {
    combineLatest([this.search$])
      .pipe(this.untilDisconnect())
      .subscribe(() => this.refreshData());

    return super.connect(collectionViewer);
  }

  setSearch(s: string): void {
    this._search$.next(s?.trim() || null);
  }

  protected fetchPage(pageIdx: number, pageSize: number): Observable<ApiContract<PagedList<WebhookRowData>>> {
    return this.webhooksService.webhooksGetPage(this._search$.value, pageIdx, null, pageSize);
  }

}
