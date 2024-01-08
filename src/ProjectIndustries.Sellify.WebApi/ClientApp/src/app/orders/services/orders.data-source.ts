import {InfiniteScrollDataSourceBase} from "../../core/services/infinite-scroll.data-source-base";
import {OrderRowData, OrdersService, OrderStatus} from "../../sellify-api";
import {PagedList} from "../../core/models/paged-list.model";
import {ApiContract} from "../../core/models/api-contract.model";
import {Observable} from "rxjs/internal/Observable";
import {Injectable} from "@angular/core";
import {BehaviorSubject, combineLatest} from "rxjs";
import {debounceTime, distinctUntilChanged} from "rxjs/operators";
import {CollectionViewer} from "@angular/cdk/collections";

@Injectable()
export class OrdersDataSource extends InfiniteScrollDataSourceBase<OrderRowData> {
  private _search$ = new BehaviorSubject<string>(null);
  private _status$ = new BehaviorSubject<OrderStatus>(null);
  private _startAt$ = new BehaviorSubject<string>(null);
  private _endAt$ = new BehaviorSubject<string>(null);

  search$ = this._search$.asObservable().pipe(distinctUntilChanged());

  status$ = this._status$.asObservable().pipe(distinctUntilChanged());

  startAt$ = this._startAt$.asObservable().pipe(distinctUntilChanged());
  endAt$ = this._endAt$.asObservable().pipe(distinctUntilChanged());

  constructor(private ordersService: OrdersService) {
    super();
  }

  setSearch(s: string): void {
    this._search$.next(s?.trim() || null);
  }

  setStatus(s?: OrderStatus): void {
    this._status$.next(s);
  }

  setPeriod(startAt: string, endAt: string): void {
    this._startAt$.next(startAt);
    this._endAt$.next(endAt);
  }

  protected fetchPage(pageIdx: number, pageSize: number): Observable<ApiContract<PagedList<OrderRowData>>> {
    return this.ordersService.ordersGetRowsPage(this._status$.value, this._startAt$.value, this._endAt$.value,
      this._search$.value, pageIdx, null, pageSize);
  }
}
