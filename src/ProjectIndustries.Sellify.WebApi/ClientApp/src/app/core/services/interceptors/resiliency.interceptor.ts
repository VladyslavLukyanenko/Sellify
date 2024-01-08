import {
  HTTP_INTERCEPTORS,
  HttpEvent,
  HttpEventType,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from "@angular/common/http";
import {Observable, throwError, timer} from "rxjs";
import {mergeMap, retryWhen} from "rxjs/operators";
import {Injectable} from "@angular/core";
import {getLogger} from "../logging.service";


const retryStrategy = ({maxRetryAttempts = 5, scalingDuration = 1000, includedStatusCodes = [500, 502, 504]} = {}) => {
  const retryStrategyLogger = getLogger("retryStrategy");
  return (attempts: Observable<any>) =>
    attempts.pipe(
      mergeMap((error, idx) => {
          const currentAttempt = idx + 1;
          if (currentAttempt > maxRetryAttempts) {
            retryStrategyLogger.error("Maximum retry attempts reached");
            return throwError(error);
          } else if (includedStatusCodes.indexOf(error.status) === -1) {
            retryStrategyLogger.debug("Retry attempt skipped.");
            return throwError(error);
          }

          const delay = currentAttempt * scalingDuration;
          retryStrategyLogger.warn(`Retry in ${delay}ms...`);
          return timer(delay);
        }
      )
    );
};

@Injectable({
  providedIn: "root"
})
export class ResiliencyInterceptor implements HttpInterceptor {
  interceptorLogger = getLogger("ResiliencyInterceptor");
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return new Observable<HttpEvent<any>>(observer => {
      next.handle(req)
        .pipe(
          retryWhen(retryStrategy())
        )
        .subscribe(r => {
            observer.next(r);
            if (r.type === HttpEventType.Response) {
              observer.complete();
            }
          },
          e => {
            observer.error(e);
            observer.complete();
          });
    });
  }
}

export const ResiliencyInterceptorProvider = {
  multi: true,
  useClass: ResiliencyInterceptor,
  provide: HTTP_INTERCEPTORS
};
