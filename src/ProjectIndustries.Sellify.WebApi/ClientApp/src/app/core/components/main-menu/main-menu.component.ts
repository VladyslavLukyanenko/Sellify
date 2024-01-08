import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
  ViewChild
} from "@angular/core";
import {DisposableComponentBase} from "../../../shared/components/disposable.component-base";

interface MenuItem {
  route: string;
  title: string;
  icon: string;
}

@Component({
  selector: "app-main-menu",
  templateUrl: "./main-menu.component.html",
  styleUrls: ["./main-menu.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MainMenuComponent extends DisposableComponentBase implements OnInit, OnDestroy {
  @Output()
  navigated = new EventEmitter<void>();

  @ViewChild("menu", {static: true}) menu: ElementRef<HTMLElement>;
  menuItems: MenuItem[] = [
    {
      route: "/dashboard",
      title: "Dashboard",
      icon: "#dashboard"
    },
    {
      route: "/orders",
      title: "Orders",
      icon: "#orders"
    },
    {
      route: "/customers",
      title: "Customers",
      icon: "#customers"
    },
    {
      route: "/products",
      title: "Products",
      icon: "#products"
    },
    {
      route: "/store-settings",
      title: "Store settings",
      icon: "#store-settings"
    }
  ];

  trackByRoute = (_: number, item: MenuItem) => item.route;

  ngOnInit(): void {
    this.menu.nativeElement.addEventListener("click", this.dispatchNavigated, true);
  }

  ngOnDestroy(): void {
    super.ngOnDestroy();
    this.menu.nativeElement.removeEventListener("click", this.dispatchNavigated, true);
  }

  private dispatchNavigated = () => {
    this.navigated.emit();
  }
}
