import {InfiniteScrollDataSourceBase} from "../../core/services/infinite-scroll.data-source-base";
import {OrdersService, PurchasedProductData} from "../../sellify-api";
import {Observable} from "rxjs/internal/Observable";
import {ApiContract} from "../../core/models/api-contract.model";
import {PagedList} from "../../core/models/paged-list.model";
import {Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs";

@Injectable()
export class PurchasedOrdersDataSource extends InfiniteScrollDataSourceBase<PurchasedProductData> {
  customerId$ = new BehaviorSubject<number>(null);
  constructor(private ordersService: OrdersService) {
    super();
  }

  protected fetchPage(pageIdx: number, pageSize: number): Observable<ApiContract<PagedList<PurchasedProductData>>> {
    return this.ordersService.ordersGetPurchasesPage(this.customerId$.value, pageIdx, null, pageSize);
  }
}
