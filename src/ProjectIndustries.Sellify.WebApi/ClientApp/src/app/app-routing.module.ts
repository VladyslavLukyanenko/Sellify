import {RouterModule, Routes} from "@angular/router";
import {NgModule} from "@angular/core";
import {LayoutComponent} from "./core/components/layout/layout.component";
import {NotFoundComponent} from "./core/components/not-found/not-found.component";
import {AuthorizeGuard} from "./api-authorization/authorize.guard";

const appRoutes: Routes = [
  {
    path: "",
    redirectTo: "dashboard",
    pathMatch: "full"
  },
  {
    path: "",
    component: LayoutComponent,
    runGuardsAndResolvers: "always",
    canActivate: [AuthorizeGuard],
    children: [

      {
        path: "dashboard",
        loadChildren: () => import("./dashboard/dashboard.module").then(_ => _.DashboardModule)
      },
      {
        path: "orders",
        loadChildren: () => import("./orders/orders.module").then(_ => _.OrdersModule)
      },
      {
        path: "customers",
        loadChildren: () => import("./customers/customers.module").then(_ => _.CustomersModule)
      },
      {
        path: "products",
        loadChildren: () => import("./products/products.module").then(_ => _.ProductsModule)
      },
      {
        path: "store-settings",
        loadChildren: () => import("./store-settings/store-settings.module").then(_ => _.StoreSettingsModule)
      },
      {
        path: "public-store",
        loadChildren: () => import("./public/public.module").then(_ => _.PublicModule)
      }
    ]
  },
  {
    path: "**",
    component: NotFoundComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes, {onSameUrlNavigation: "reload"})
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {
}
