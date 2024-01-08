import {Component, OnInit, ChangeDetectionStrategy, Input, Output, EventEmitter} from "@angular/core";
import * as moment from "moment";
import {KeyValuePair} from "../../../core/models/key-value-pair.model";
import {NumsUtil} from "../../../core/services/nums.util";


export enum GeneralPeriodTypes {
  Yearly = "Yearly",
  Monthly = "Monthly",
  AllTime = "AllTime"
}

export const monthFormat = "yyyy-MM";

const maxYear = (new Date()).getFullYear();
const minYear = 2020;
const years = [...new Array(maxYear - minYear + 1)]
  .map((_, idx) => ({
    key: minYear + idx,
    value: minYear + idx
  }))
  .reverse();

export const dateFormats = {
  [GeneralPeriodTypes.Yearly]: "yy",
  [GeneralPeriodTypes.Monthly]: "yy-mm"
};


export const yearMonthPattern = /\d{4}-\d{2}/;
export const isStartAtValid = (period: GeneralPeriodTypes, startAt: string) => {
  return period === GeneralPeriodTypes.AllTime
    || period === GeneralPeriodTypes.Yearly && !isNaN(+startAt)
    || period === GeneralPeriodTypes.Monthly && yearMonthPattern.test(startAt);
};

const now = new Date();
export const startAtDefaults = {
  [GeneralPeriodTypes.Monthly]: `${now.getFullYear()}-${NumsUtil.toLeftPaddedStr(now.getMonth() + 1)}`,
  [GeneralPeriodTypes.Yearly]: now.getFullYear()
};

@Component({
  selector: "app-period-picker",
  templateUrl: "./period-picker.component.html",
  styleUrls: ["./period-picker.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PeriodPickerComponent {
  @Input() period!: GeneralPeriodTypes;
  @Input() startAt!: number | string;

  @Output() periodChange = new EventEmitter<GeneralPeriodTypes>();
  @Output() startAtChange = new EventEmitter<number | string>();

  years = years;
  periods = [
    {value: GeneralPeriodTypes.Monthly, key: "Monthly"},
    {value: GeneralPeriodTypes.Yearly, key: "Yearly"},
    {value: GeneralPeriodTypes.AllTime, key: "All time"},
  ] as KeyValuePair<string, GeneralPeriodTypes>[];


  get dateFormat(): string {
    return dateFormats[this.period];
  }

  get isMonthlySelected(): boolean {
    return this.period === GeneralPeriodTypes.Monthly;
  }

  get isYearlySelected(): boolean {
    return this.period === GeneralPeriodTypes.Yearly;
  }

  dispatchPeriodChange(p: GeneralPeriodTypes): void {
    this.periodChange.emit(p);
  }

  dispatchStartAtChange(p: string | Date): void {
    if (p instanceof Date) {
      this.startAtChange.emit(moment(p).format(monthFormat));
      return;
    }

    this.startAtChange.emit(p);
  }
}
