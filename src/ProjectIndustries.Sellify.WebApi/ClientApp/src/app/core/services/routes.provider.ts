// import {Injectable} from "@angular/core";
// import {BehaviorSubject} from "rxjs";
// import {RouteData} from "../models/route-data.model";
// import {ActivatedRoute, Routes} from "@angular/router";
// import {TokenService} from "./token.service";
// import {ClaimNames} from "../models/claim-names.model";
//
//
// const lastLoggedInDashboardDomain = "space.dashboards.lastloggedin.domain";
// const lastLoggedInDashboardMode = "space.dashboards.lastloggedin.mode";
//
// @Injectable({
//   providedIn: "root"
// })
// export class RoutesProvider {
//   private _rootRoutes$ = new BehaviorSubject<RouteData[]>([]);
//   private _secondLvlRoutes$ = new BehaviorSubject<RouteData[]>([]);
//   rootRoutes$ = this._rootRoutes$.asObservable();
//   secondLvlRoutes$ = this._secondLvlRoutes$.asObservable();
//
//   constructor(private tokenService: TokenService, private activatedRoute: ActivatedRoute) {
//     tokenService.encodedAccessToken$.subscribe(t => {
//       if (t) {
//         const token = tokenService.decodedAccessToken;
//         localStorage.setItem(lastLoggedInDashboardMode, token[ClaimNames.currentDashboardHostingMode]);
//         localStorage.setItem(lastLoggedInDashboardDomain, token[ClaimNames.currentDashboardDomain]);
//       }
//     });
//   }
//
//   setRootRoutes(routes: RouteData[]) {
//     this._rootRoutes$.next(routes);
//   }
//
//   setSecondLvlRoutes(routes: RouteData[]) {
//     this._secondLvlRoutes$.next(routes);
//   }
//
//   extractRouteDataList(routes: Routes): RouteData[] {
//     return routes.map(r => <RouteData>r.data).filter(r => r && r instanceof RouteData);
//   }
//
//   resolveUrlFromRoot(...segments: string[]) {
//     return ["/", ...segments.filter(r => !!r)];
//   }
//
//   getLoginUrl() {
//     const m = /\/account\/login\/(\w+)\/?/.exec(location.pathname);
//     let domain = m && m[1] || "";
//     if (localStorage.getItem(lastLoggedInDashboardMode) === "PathSegment") {
//       domain = localStorage.getItem(lastLoggedInDashboardDomain) || "";
//     }
//
//     return ["/account", "login", domain].filter(t => !!t);
//   }
//
//   getAuthenticatedRedirectUrl() {
//     return ["/analytics"];
//   }
// }
