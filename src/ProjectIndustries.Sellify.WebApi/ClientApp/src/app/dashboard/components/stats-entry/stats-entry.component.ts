import {Component, OnInit, ChangeDetectionStrategy, Input} from "@angular/core";

@Component({
  selector: "app-stats-entry",
  templateUrl: "./stats-entry.component.html",
  styleUrls: ["./stats-entry.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StatsEntryComponent implements OnInit {
  @Input() title: string;
  @Input() displayValue: any;
  @Input() changesPercent: number;

  constructor() { }

  ngOnInit(): void {
  }

}
