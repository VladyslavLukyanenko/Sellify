import {ChangeDetectionStrategy, Component, ViewEncapsulation} from "@angular/core";

@Component({
  selector: "app-subscription-plan-widget",
  templateUrl: "./subscription-plan-widget.component.html",
  styleUrls: ["./subscription-plan-widget.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class SubscriptionPlanWidgetComponent {
  isUpgradeDialogVisible = false;
}
