import {InfiniteScrollDataSourceBase} from "../../core/services/infinite-scroll.data-source-base";
import {CustomerRowData, CustomersService, OrdersService, OrderStatus} from "../../sellify-api";
import {Injectable} from "@angular/core";
import {PagedList} from "../../core/models/paged-list.model";
import {ApiContract} from "../../core/models/api-contract.model";
import {Observable} from "rxjs/internal/Observable";
import {BehaviorSubject} from "rxjs";
import {distinctUntilChanged} from "rxjs/operators";

@Injectable()
export class CustomersDataSource extends InfiniteScrollDataSourceBase<CustomerRowData> {
  private _search$ = new BehaviorSubject<string>(null);
  private _startAt$ = new BehaviorSubject<string>(null);
  private _endAt$ = new BehaviorSubject<string>(null);

  search$ = this._search$.asObservable().pipe(distinctUntilChanged());
  startAt$ = this._startAt$.asObservable().pipe(distinctUntilChanged());
  endAt$ = this._endAt$.asObservable().pipe(distinctUntilChanged());

  constructor(private customerService: CustomersService) {
    super();
  }

  setSearch(s: string): void {
    this._search$.next(s?.trim() || null);
  }

  setPeriod(startAt: string, endAt: string): void {
    this._startAt$.next(startAt);
    this._endAt$.next(endAt);
  }

  protected fetchPage(pageIdx: number, pageSize: number): Observable<ApiContract<PagedList<CustomerRowData>>> {
    return this.customerService.customersGetCustomerRowsPage(this._startAt$.value, this._endAt$.value,
      this._search$.value, pageIdx, null, pageSize);
  }

}
