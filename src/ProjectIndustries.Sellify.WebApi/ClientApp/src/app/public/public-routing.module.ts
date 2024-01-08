import {RouterModule, Routes} from "@angular/router";
import {NgModule} from "@angular/core";
import {HomeComponent} from "./home/home.component";
import {ProductComponent} from "./product/product.component";
import {CheckoutComponent} from "./checkout/checkout.component";
import {CurrentStoreResolver} from "./current-store.resolver";

const routes: Routes = [
  {
    path: ":storeDomain",
    resolve: {
      storeInfo: CurrentStoreResolver
    },
    children: [
      {
        path: "",
        pathMatch: "full",
        component: HomeComponent
      },
      {
        path: ":productId",
        component: ProductComponent
      },
      {
        path: ":productId/checkout",
        component: CheckoutComponent
      }
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class PublicRoutingModule {
}
