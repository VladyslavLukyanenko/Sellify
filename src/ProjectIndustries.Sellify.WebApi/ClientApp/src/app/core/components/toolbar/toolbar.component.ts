import {ChangeDetectionStrategy, Component, EventEmitter, Input, Output} from "@angular/core";
import {DisposableComponentBase} from "../../../shared/components/disposable.component-base";
import {LayoutService} from "../../services/layout.service";

@Component({
  selector: "app-toolbar",
  templateUrl: "./toolbar.component.html",
  styleUrls: ["./toolbar.component.scss"],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToolbarComponent extends DisposableComponentBase {
  @Input()
  sidebarOpened!: boolean;

  @Output()
  sidebarOpenedChange = new EventEmitter<boolean>();

  constructor(readonly layoutService: LayoutService) {
    super();
  }

  logOut(): void {
  }
}
