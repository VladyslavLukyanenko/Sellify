import {NgModule} from "@angular/core";
import {HomeComponent} from "./home/home.component";
import {ProductComponent} from "./product/product.component";
import {CheckoutComponent} from "./checkout/checkout.component";
import {StoreHostComponent} from "./store-host/store-host.component";
import {SharedModule} from "../shared/shared.module";
import {PublicRoutingModule} from "./public-routing.module";
import {PayPalModule} from "../paypal";


@NgModule({
  declarations: [HomeComponent, ProductComponent, CheckoutComponent, StoreHostComponent],
  imports: [
    SharedModule,
    PublicRoutingModule,

    PayPalModule.init({
      clientId: "sb", // Using sandbox for testing purposes only
      currency: "USD",
      integrationDate: "2020-11-01"
      // merchantId: "abc"
      // commit: true,
      // vault: true,
      // disableFunding: "card"
    })
  ]
})

export class PublicModule {
}
