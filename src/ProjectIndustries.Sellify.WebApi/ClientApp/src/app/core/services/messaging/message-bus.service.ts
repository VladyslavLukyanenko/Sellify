import {Injectable, Type} from "@angular/core";
import {Subscription} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class MessageBusService {
  private holders: MessageEventHandlersHolder[] = [];

  subscribe<T>(messageType: Type<T>, callback: (arg: T) => void): Subscription {
    let holder = this.holders.find(_ => _.messageType === messageType);
    if (!holder) {
      holder = new MessageEventHandlersHolder(messageType);
      this.holders.push(holder);
    }

    const handler = new MessageBusEventHandler(messageType, callback);
    holder.add(handler);
    return new Subscription(() => holder?.remove(handler));
  }

  broadcast<T>(messageType: Type<T>, arg: T): void {
    const holder = this.holders.find(_ => _.messageType === messageType);
    if (!holder) {
      return;
    }

    for (const cur of holder.handlers) {
      cur.callback(arg);
    }
  }
}

class MessageEventHandlersHolder {
  readonly handlers: MessageBusEventHandler[];

  constructor(readonly messageType: Type<any>) {
    this.handlers = [];
  }

  add(handler: MessageBusEventHandler): void {
    this.handlers.push(handler);
  }

  remove(handler: MessageBusEventHandler): void {
    const idx = this.handlers.indexOf(handler);
    if (idx > -1) {
      this.handlers.splice(idx, 1);
    }
  }
}

class MessageBusEventHandler {
  constructor(readonly messageType: Type<any>, readonly callback: (...args: any[]) => any) {
  }
}
