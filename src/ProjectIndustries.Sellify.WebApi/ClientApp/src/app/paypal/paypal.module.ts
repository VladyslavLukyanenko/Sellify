import {NgModule, ModuleWithProviders, Inject, Optional} from "@angular/core";
import {getPayPal, loadPayPalSdk, PAYPAL_CONFIG, PAYPAL_INSTANCE} from "./paypal-factory";
import {PayPal, PayPalConfig} from "./types/paypal";
import {PayPalButtons} from "./paypal.component";

/** Global script loading promise to be use as initializer */
let $paypal: Promise<PayPal>;

@NgModule({
  declarations: [PayPalButtons],
  exports: [PayPalButtons]
})
export class PayPalModule {

  constructor(@Optional() @Inject(PAYPAL_CONFIG) config: PayPalConfig) {

    if (!config) {
      throw new Error(`
      PayPal module has not been initialized.
      Make sure to call PayPalModule.init(...) in your root module.
    `);
    }

    // Triggers the paypal.js API loading asyncronously.
    $paypal = loadPayPalSdk(config);
  }

  static init(config: PayPalConfig): ModuleWithProviders<PayPalModule> {
    return {
      ngModule: PayPalModule,
      providers: [
        /** Provides the global PayPalConfig object */
        {provide: PAYPAL_CONFIG, useValue: config},
        /** Provides the global paypal instance */
        {provide: PAYPAL_INSTANCE, useFactory: () => $paypal}
      ]
    };
  }
}
