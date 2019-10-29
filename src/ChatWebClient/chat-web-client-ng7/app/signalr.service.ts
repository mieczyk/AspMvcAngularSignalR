import { Injectable } from '@angular/core';
import { SignalR, ISignalRConnection } from 'ng2-signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
    private connection: Promise<ISignalRConnection>;

    constructor(private _signalR: SignalR) {
        this.connection = _signalR.connect();
    }

    public getOpenConnection() {
        return this.connection;
    }
}
