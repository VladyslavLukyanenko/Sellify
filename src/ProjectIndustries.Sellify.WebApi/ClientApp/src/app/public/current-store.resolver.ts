import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from "@angular/router";
import {Observable} from "rxjs/internal/Observable";
import {StoreInfoData, StoresService} from "../sellify-api";
import {map, tap} from "rxjs/operators";
import {Injectable} from "@angular/core";

let lastLoadedStoreInfo: StoreInfoData = null;
let storeDomain: string = null;

@Injectable({
  providedIn: "root"
})
export class CurrentStoreResolver implements Resolve<StoreInfoData> {
  constructor(private storeService: StoresService) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<StoreInfoData> | StoreInfoData {
    const currStoreDomain = route.params.storeDomain;
    if (currStoreDomain === storeDomain && lastLoadedStoreInfo) {
      return lastLoadedStoreInfo;
    }

    storeDomain = currStoreDomain;
    return this.storeService.storesGetStoreInfo(storeDomain)
      .pipe(
        map(_ => _.payload),
        tap(it => lastLoadedStoreInfo = it)
      );
  }
}
