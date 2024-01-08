import {ChangeDetectionStrategy, Component, OnInit, TemplateRef, ViewChild, ViewContainerRef} from "@angular/core";
import {LayoutService} from "../../../core/services/layout.service";
import {DisposableComponentBase} from "../../../shared/components/disposable.component-base";
import {TemplatePortal} from "@angular/cdk/portal";
import {EChartsOption} from "echarts";


import symbol from "../../../../assets/chart-symbol.svg";
import {AnalyticsService, DecimalValueDiff, Int32ValueDiff} from "../../../sellify-api";
import {BehaviorSubject} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
import {NumsUtil} from "../../../core/services/nums.util";
import {ToolbarService} from "../../../core/services/toolbar.service";
import * as moment from "moment";
import {
  GeneralPeriodTypes,
  isStartAtValid,
  startAtDefaults
} from "../../../shared/components/period-picker/period-picker.component";


interface RevenueEntry {
  name: string;
  value: [Date, number];
}

const revenueOptions: EChartsOption = {
  color: ["#0074FF"],
  title: {
    show: false,
  },
  grid: {
    left: 40,
    top: 50,
    right: 0,
    bottom: 30
  },
  tooltip: {
    trigger: "axis",
    backgroundColor: null,
    className: "FloatingTooltip",
    position: (p) => [p[0] + 15, p[1] + 10],
    borderColor: null,
    borderWidth: 0,
    formatter: (params, ticket) => {
      const item = params[0];
      return `
    <span class="FloatingTooltip-wrap">
      <span class="FloatingTooltip-title">${item.data.name}</span>
      <span class="FloatingTooltip-value">\$${item.data.value[1]}</span>
    </span>`;
    },
    textStyle: {
      fontFamily: "Poppins, sans-serif",
    }
  },
  xAxis: {
    type: "time",
    boundaryGap: false,
    splitLine: {
      show: true
    }
  },
  yAxis: {
    type: "value",
    splitLine: {
      show: true
    }
  },
  series: [{
    data: [] as RevenueEntry[],
    type: "line",
    smooth: true,
    showSymbol: false,
    symbolOffset: [0, 4],
    symbolSize: 40,
    symbol: "image://" + location.origin + "/" + symbol,
    lineStyle: {
      width: 3
    },
    areaStyle: {
      color: {
        type: "linear",
        x: 0,
        y: 0,
        x2: 0,
        y2: 1,
        global: false,
        __canvasGradient: null,
        colorStops: [{
          offset: 1,
          color: "rgba(218, 235, 255, 0.8)" // color at 0% position
        }, {
          offset: 0,
          color: "rgba(158, 202, 255, 1)" // color at 100% position
        }],
      }
    },
  }]
};


const nullo = Object.create({
  current: 0,
  previous: 0,
  changePercents: 0
});

@Component({
  selector: "app-analytics-page",
  templateUrl: "./analytics-page.component.html",
  styleUrls: ["./analytics-page.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: "Page"
  }
})
export class AnalyticsPageComponent extends DisposableComponentBase implements OnInit {
  @ViewChild("createProductTmpl", {static: true}) createProductTmpl: TemplateRef<any>;

  period$ = new BehaviorSubject<GeneralPeriodTypes>(GeneralPeriodTypes.AllTime);
  startAt$ = new BehaviorSubject<number | string>(startAtDefaults[this.period$.value]);

  revenueOptions$ = new BehaviorSubject<any>({...revenueOptions});

  totalRevenue$ = new BehaviorSubject<DecimalValueDiff>(nullo);
  totalCustomers$ = new BehaviorSubject<Int32ValueDiff>(nullo);
  totalOrders$ = new BehaviorSubject<Int32ValueDiff>(nullo);
  visitorsCount$ = new BehaviorSubject<Int32ValueDiff>(nullo);

  constructor(private layoutService: LayoutService,
              private viewContainerRef: ViewContainerRef,
              private router: Router,
              private activatedRoute: ActivatedRoute,
              private analyticsService: AnalyticsService,
              private toolbarService: ToolbarService) {
    super();
    toolbarService.setTitle("Analytics");
  }

  ngOnInit(): void {
    this.layoutService
      .displayOnSidebar(new TemplatePortal(this.createProductTmpl, this.viewContainerRef), this.destroy$);

    this.activatedRoute.queryParams
      .pipe(
        this.untilDestroy()
      )
      .subscribe(async p => {
          const period = p.period;
          const startAt = period === GeneralPeriodTypes.Yearly ? +p.startAt : p.startAt;

          this.startAt$.next(startAt);
          this.period$.next(period);
          this.totalRevenue$.next(nullo);
          this.totalCustomers$.next(nullo);
          this.totalOrders$.next(nullo);
          this.revenueOptions$.next({...revenueOptions});
          this.visitorsCount$.next(nullo);

          if (!period || !(period in GeneralPeriodTypes) || !isStartAtValid(period, startAt)) {
            await this.router.navigate([], {
              relativeTo: this.activatedRoute,
              queryParams: {period: GeneralPeriodTypes.Yearly, startAt: (new Date()).getFullYear()},
              replaceUrl: true
            });
            return;
          }

          const pad = "00";
          const offsetHours = (new Date()).getTimezoneOffset() / 60 * -1;
          const h = Math.abs(Math.floor(offsetHours));
          const m = Math.abs(Math.abs(offsetHours) - h);
          const offset = `${offsetHours > 0 ? "+" : "-"}${NumsUtil.toLeftPaddedStr(h, pad)}:${NumsUtil.toLeftPaddedStr(m, pad)}`;

          const r = await this.asyncTracker.executeAsAsync(
            this.analyticsService.analyticsGetGeneral(offset, startAt, period));
          const data = r.payload;

          this.totalRevenue$.next(data.totalRevenue);
          this.totalCustomers$.next(data.totalCustomers);
          this.totalOrders$.next(data.totalOrders);
          this.visitorsCount$.next(data.visitorsCount);

          const incomeData = data.income.map(_ => {
            const parsedDate = moment(_.date, "YYYY-MM-DD");
            return ({
              name: parsedDate.format("DD-MM-yyyy"),
              value: [
                parsedDate.toDate(),
                _.amount
              ]
            });
          });

          this.revenueOptions$.next({
            ...revenueOptions,
            series: [
              {
                ...revenueOptions.series[0],
                data: incomeData
              }
            ]
          });
        }
      );
  }

  pushStartAt(startAt: string | number): Promise<any> {
    return this.router.navigate([], {
      queryParams: {
        startAt
      },
      queryParamsHandling: "merge",
      relativeTo: this.activatedRoute
    });
  }

  pushPeriod(period: GeneralPeriodTypes): Promise<any> {
    return this.router.navigate([], {
      queryParams: {
        period,
        startAt: startAtDefaults[period]
      },
      queryParamsHandling: "merge",
      relativeTo: this.activatedRoute
    });
  }
}

export interface ActiveFilter {
  label: string;
  value: any;
  kind: string;
  command: () => Promise<any> | any;
}
