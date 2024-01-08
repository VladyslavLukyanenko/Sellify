import {Injectable, TemplateRef} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {Portal} from "@angular/cdk/portal";
import {Observable} from "rxjs/internal/Observable";
import {take} from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class LayoutService {
  private _sidebarExtraSpacePortal$ = new BehaviorSubject<Portal<any>>(null);
  readonly sidebarExtraSpacePortal$ = this._sidebarExtraSpacePortal$.asObservable();

  private _toolbarExtraSpacePortal$ = new BehaviorSubject<Portal<any>>(null);
  readonly toolbarExtraSpacePortal$ = this._toolbarExtraSpacePortal$.asObservable();

  displayOnToolbar(portal: Portal<any>, disposer: Observable<any>): void {
    disposer.pipe(take(1))
      .subscribe(() => this._toolbarExtraSpacePortal$.next(null));
    this._toolbarExtraSpacePortal$.next(portal);
  }

  displayOnSidebar(portal: Portal<any>, disposer: Observable<any>): void {
    disposer.pipe(take(1))
      .subscribe(() => this._sidebarExtraSpacePortal$.next(null));
    this._sidebarExtraSpacePortal$.next(portal);
  }
}
