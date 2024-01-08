import {BrowserModule} from "@angular/platform-browser";
import {NgModule} from "@angular/core";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";

import {AppRoutingModule} from "./app-routing.module";

import {CoreModule} from "./core/core.module";


import {AppComponent} from "./app.component";
import {SharedModule} from "./shared/shared.module";
import {environment} from "../environments/environment";
import {BASE_PATH as MONOLITHIC_API_BASE_PATH} from "./sellify-api";
import {ResiliencyInterceptorProvider} from "./core/services/interceptors/resiliency.interceptor";
import {ActivityInterceptorProvider} from "./core/services/interceptors/activity.interceptor";
import {AuthorizeInterceptor} from "./api-authorization/authorize.interceptor";
import {QuillModule} from "ngx-quill";


@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    CoreModule,
    BrowserModule.withServerTransition({appId: "ng-cli-universal"}),

    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,

    QuillModule.forRoot({
      modules: {
        toolbar: [
          ["bold", "italic", { header: 1}, "image", "link", { list: "ordered"}, { list: "bullet" }, "blockquote", "code-block"]
        ]
      }
    }),

    SharedModule,
    AppRoutingModule,
  ],

  providers: [
    {provide: MONOLITHIC_API_BASE_PATH, useValue: environment.apiHostUrl},
    ActivityInterceptorProvider,
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    ResiliencyInterceptorProvider,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
