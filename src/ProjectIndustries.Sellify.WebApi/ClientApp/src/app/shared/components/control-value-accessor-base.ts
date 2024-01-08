﻿import {DisposableComponentBase} from "./disposable.component-base";
import {Directive, Injector, OnInit} from "@angular/core";
import {AbstractControl, ControlContainer, ControlValueAccessor, FormControl, NgControl} from "@angular/forms";
import {distinctUntilChanged, takeWhile} from "rxjs/operators";

const noop = (_?: any) => {
};
type ValueChangesObserverFn = (newValue?: any) => void;

@Directive()
// tslint:disable-next-line:directive-class-suffix
export abstract class ControlValueAccessorBase extends DisposableComponentBase implements OnInit, ControlValueAccessor {
  protected valueChangeObserver = noop;
  protected onTouchedObserver = noop;

  control: AbstractControl;

  protected constructor(private injector: Injector) {
    super();
  }

  ngOnInit(): void {
    const ngControl = this.injector.get(NgControl, null);
    if (ngControl) {
      ngControl.valueAccessor = this;
    }

    const container = this.injector.get(ControlContainer, null);
    this.control = container?.control || new FormControl();

    this.control.statusChanges
      .pipe(
        this.untilDestroy(),
        takeWhile(() => !this.control.touched)
      )
      .subscribe(() => {
        this.onTouchedObserver();
      });
    this.control.valueChanges
      .pipe(
        this.untilDestroy(),
        distinctUntilChanged()
      )
      .subscribe(v => {
        this.valueChangeObserver(v);
      });
  }

  registerOnChange(fn: ValueChangesObserverFn): void {
    this.valueChangeObserver = fn;
  }

  registerOnTouched(fn: ValueChangesObserverFn): void {
    this.onTouchedObserver = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    isDisabled ? this.control.disable() : this.control.enable();
  }

  writeValue(obj: any): void {
    this.control.patchValue(obj);
  }
}
