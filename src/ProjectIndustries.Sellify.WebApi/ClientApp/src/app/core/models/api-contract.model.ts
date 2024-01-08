import {ApiError} from "../../sellify-api";

export interface ApiContract<T> {
  error?: ApiError;
  readonly payload?: T | null;
}
