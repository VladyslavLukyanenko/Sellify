import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { Configuration } from './configuration';
import { HttpClient } from '@angular/common/http';

import { AnalyticsService } from './api/analytics.service';
import { CategoriesService } from './api/categories.service';
import { CustomersService } from './api/customers.service';
import { OidcConfigurationService } from './api/oidc-configuration.service';
import { OrdersService } from './api/orders.service';
import { PayPalService } from './api/pay-pal.service';
import { PaymentsService } from './api/payments.service';
import { ProductsService } from './api/products.service';
import { SkrillService } from './api/skrill.service';
import { StoresService } from './api/stores.service';
import { StripeService } from './api/stripe.service';
import { WebhooksService } from './api/webhooks.service';

@NgModule({
  imports:      [],
  declarations: [],
  exports:      [],
  providers: []
})
export class ApiModule {
    public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders<ApiModule> {
        return {
            ngModule: ApiModule,
            providers: [ { provide: Configuration, useFactory: configurationFactory } ]
        };
    }

    constructor( @Optional() @SkipSelf() parentModule: ApiModule,
                 @Optional() http: HttpClient) {
        if (parentModule) {
            throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
        }
        if (!http) {
            throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
            'See also https://github.com/angular/angular/issues/20575');
        }
    }
}
