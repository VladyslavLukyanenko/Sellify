export * from './analytics.service';
import { AnalyticsService } from './analytics.service';
export * from './categories.service';
import { CategoriesService } from './categories.service';
export * from './customers.service';
import { CustomersService } from './customers.service';
export * from './oidc-configuration.service';
import { OidcConfigurationService } from './oidc-configuration.service';
export * from './orders.service';
import { OrdersService } from './orders.service';
export * from './pay-pal.service';
import { PayPalService } from './pay-pal.service';
export * from './payments.service';
import { PaymentsService } from './payments.service';
export * from './products.service';
import { ProductsService } from './products.service';
export * from './skrill.service';
import { SkrillService } from './skrill.service';
export * from './stores.service';
import { StoresService } from './stores.service';
export * from './stripe.service';
import { StripeService } from './stripe.service';
export * from './webhooks.service';
import { WebhooksService } from './webhooks.service';
export const APIS = [AnalyticsService, CategoriesService, CustomersService, OidcConfigurationService, OrdersService, PayPalService, PaymentsService, ProductsService, SkrillService, StoresService, StripeService, WebhooksService];
