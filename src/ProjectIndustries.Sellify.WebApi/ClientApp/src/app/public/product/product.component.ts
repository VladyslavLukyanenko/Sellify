import {Component, OnInit, ChangeDetectionStrategy, forwardRef} from "@angular/core";
import {DisposableComponentBase} from "../../shared/components/disposable.component-base";
import {ActivatedRoute} from "@angular/router";
import {ToolbarService} from "../../core/services/toolbar.service";
import {
  CreateOrderCommand,
  OrdersService,
  PaymentGateway,
  PaymentsService, PayPalService,
  ProductData,
  ProductsService, SkrillService, StoreInfoData,
  StripeService
} from "../../sellify-api";
import {map} from "rxjs/operators";
import {BehaviorSubject} from "rxjs";
import {loadStripe} from "@stripe/stripe-js";
import {environment} from "../../../environments/environment";
import {OnApproveActions, OnApproveData, OnCancelData, OnErrorData, OrderRequest, PayPalProcessor} from "../../paypal";

@Component({
  selector: "app-product",
  templateUrl: "./product.component.html",
  styleUrls: ["./product.component.less"],
  providers: [{provide: PayPalProcessor, useExisting: forwardRef(() => ProductComponent)}],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductComponent extends DisposableComponentBase implements OnInit {

  constructor(
    private activatedRoute: ActivatedRoute,
    private productsService: ProductsService,
    private paymentsService: PaymentsService,
    private stripeService: StripeService,
    private paypalService: PayPalService,
    private orderService: OrdersService,
    private skrillService: SkrillService,
    private toolbarService: ToolbarService) {
    super();
    toolbarService.setTitle("Store");
  }

  orderId: string;
  paymentProviders$ = new BehaviorSubject<PaymentGateway[]>([]);
  product$ = new BehaviorSubject<ProductData>(null);

  store: StoreInfoData;

  async ngOnInit(): Promise<void> {
    this.activatedRoute.params
      .pipe(this.untilDestroy())
      .subscribe(async params => {
        const productId = params.productId;
        const product = await this.asyncTracker.executeAsAsync(
          this.productsService.productsGetById(productId).pipe(map(_ => _.payload)));

        this.toolbarService.setTitle(product.title);
        this.product$.next(product);
        this.store = this.activatedRoute.snapshot.data.storeInfo;
      });

    const gateways = await this.asyncTracker.executeAsAsync(
      this.paymentsService.paymentsGetSupportedProvider().pipe(map(_ => _.payload)));
    this.paymentProviders$.next(gateways);
  }

  async purchaseWithStripe(): Promise<void> {
    const id = this.product$.value.id;
    const cmd: CreateOrderCommand = {
      productId: id,
      quantity: +prompt("Enter quantity", "1"),
      customerEmail: prompt("Enter your email", "x.alantoo@gmail.com"),
      storeId: this.store.id
    };

    const orderId = await this.asyncTracker.executeAsAsync(
      this.orderService.ordersCreateOrder(cmd).pipe(map(_ => _.payload))
    );

    const sessionId = await this.asyncTracker.executeAsAsync(
      this.stripeService.stripeCreateSession(orderId).pipe(map(_ => _.payload))
    );

    const stripe = await loadStripe(environment.payments.stripe.pkey);
    await stripe.redirectToCheckout({sessionId});
  }

  createPayPalOrder = async (): Promise<string> => {
    const id = this.product$.value.id;
    const cmd: CreateOrderCommand = {
      productId: id,
      quantity: +prompt("Enter quantity", "1"),
      customerEmail: prompt("Enter your email", "sb-9a3iq5209793@personal.example.com"),
      storeId: this.store.id
    };

    this.orderId = await this.asyncTracker.executeAsAsync(
      this.orderService.ordersCreateOrder(cmd).pipe(map(_ => _.payload))
    );

    return await this.asyncTracker.executeAsAsync(
      this.paypalService.payPalCreatePayPalOrder(this.orderId).pipe(map(_ => _.payload))
    );
  }

  createSkrillOrder = async (): Promise<void> => {
    const wnd = window.open("", "_blank", "width=400,height=600,left=0,top=0");
    const id = this.product$.value.id;
    const cmd: CreateOrderCommand = {
      productId: id,
      quantity: +prompt("Enter quantity", "1"),
      customerEmail: prompt("Enter your email", "x.alantoo@gmail.com"),
      storeId: this.store.id
    };

    this.orderId = await this.asyncTracker.executeAsAsync(
      this.orderService.ordersCreateOrder(cmd).pipe(map(_ => _.payload))
    );

    const url = await this.asyncTracker.executeAsAsync(
      this.skrillService.skrillCreateSession(this.orderId).pipe(map(_ => _.payload))
    );

    wnd.location.replace(url);
  }

  onApprove(data: OnApproveData, actions: OnApproveActions): Promise<any> {
    alert("Transaction Approved:" + data.orderID);
    return Promise.resolve();

    // Captures the trasnaction
    return this.paypalService.payPalCapture(data.orderID).toPromise();
  }

  async onCancel(data: OnCancelData): Promise<void> {
    await this.asyncTracker.executeAsAsync(this.orderService.ordersCancelOrder(this.orderId));
  }

  onError(data: OnErrorData): void {

    console.log("Transaction Error:", data);
  }
}
