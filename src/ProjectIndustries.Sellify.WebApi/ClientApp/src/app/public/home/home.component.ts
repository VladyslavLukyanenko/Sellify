import {ChangeDetectionStrategy, Component, OnInit} from "@angular/core";
import {ProductRowData, ProductsService, StoreInfoData} from "../../sellify-api";
import {BehaviorSubject} from "rxjs";
import {map} from "rxjs/operators";
import {ToolbarService} from "../../core/services/toolbar.service";
import {DisposableComponentBase} from "../../shared/components/disposable.component-base";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends DisposableComponentBase implements OnInit {

  products$ = new BehaviorSubject<ProductRowData[]>([]);

  constructor(private productsService: ProductsService,
              private activatedRoute: ActivatedRoute,
              private toolbarService: ToolbarService) {
    super();
    toolbarService.setTitle("Store");
  }

  async ngOnInit(): Promise<void> {
    const info = this.activatedRoute.snapshot.data.storeInfo as StoreInfoData;
    const storeName = info.title ? ` "${info.title}"` : "";
    this.toolbarService.setTitle(`Store${storeName}`);

    const p = await this.productsService.productsGetPage()
      .pipe(map(_ => _.payload.content))
      .toPromise();
    this.products$.next(p);
  }

}
