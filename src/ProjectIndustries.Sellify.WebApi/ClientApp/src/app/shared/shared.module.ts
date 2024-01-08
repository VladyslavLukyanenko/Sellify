import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {HttpClientModule} from "@angular/common/http";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

import {RouterModule} from "@angular/router";

import {LocalDatePipe} from "./pipes/LocalDatePipe";
import {ScrollingModule} from "@angular/cdk/scrolling";
import {DateFromNowPipe} from "./pipes/date-from-now.pipe";

import {SidebarModule} from "primeng/sidebar";
import {ButtonModule} from "primeng/button";
import {AccordionModule} from "primeng/accordion";
import {DropdownModule} from "primeng/dropdown";
import {CalendarModule} from "primeng/calendar";
import {ProgressBarModule} from "primeng/progressbar";
import {ProgressSpinnerModule} from "primeng/progressspinner";
import {MenuModule} from "primeng/menu";
import {ConfirmPopupModule} from "primeng/confirmpopup";
import {ConfirmationService} from "primeng/api";
import {DialogModule} from "primeng/dialog";
import {SkeletonModule} from "primeng/skeleton";
import {ToastModule} from "primeng/toast";
import {FieldErrorRequiredComponent} from "./components/errors/field-error-required/field-error-required.component";
import {PortalModule} from "@angular/cdk/portal";
import {SearchBarComponent} from "./components/search-bar/search-bar.component";
import {DateHumanizePipe} from "./pipes/date-humanize.pipe";
import {DatetimeHumanizePipe} from "./pipes/datetime-humanize.pipe";
import {ExpandableTableComponent} from "./components/expandable-table/expandable-table.component";
import {ConfirmDialogComponent} from "./components/confirm-dialog/confirm-dialog.component";
import {CascadeSelectModule} from "primeng/cascadeselect";
import {FileUploadComponent} from "./components/file-upload/file-upload.component";
import {PeriodPickerComponent} from "./components/period-picker/period-picker.component";

const componentsModules = [
  // InfiniteScrollModule,
  ScrollingModule,
  PortalModule,

  SidebarModule,
  CalendarModule,
  ButtonModule,
  AccordionModule,
  DropdownModule,
  ProgressBarModule,
  ProgressSpinnerModule,
  MenuModule,
  ConfirmPopupModule,
  DialogModule,
  SkeletonModule,
  ToastModule,
  CascadeSelectModule
];

const sharedSystemModules = [
  CommonModule,
  HttpClientModule,
  ReactiveFormsModule,
  FormsModule,
];

const sharedDeclarations = [
  LocalDatePipe,
  DateFromNowPipe,
  FieldErrorRequiredComponent,
  SearchBarComponent,
  DateHumanizePipe,
  DatetimeHumanizePipe,
  ExpandableTableComponent,
  ConfirmDialogComponent,
  FileUploadComponent,
  PeriodPickerComponent
];

@NgModule({
  imports: [
    RouterModule,

    ...sharedSystemModules,
    ...componentsModules
  ],
  declarations: [
    ...sharedDeclarations,
  ],
  entryComponents: [],
  exports: [
    ...sharedSystemModules,
    ...componentsModules,
    ...sharedDeclarations,
  ],
  providers: [
    ConfirmationService
  ]
})
export class SharedModule {
}
