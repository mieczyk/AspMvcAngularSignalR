import { Component } from '@angular/core';
import { SignalR, SignalRConnection } from 'ng2-signalr';

@Component({
    selector: 'chat-sender',
    templateUrl: './chat-sender.component.html',
    styles: []
})
export class ChatSender {
    constructor(private _signalR: SignalR) {}

    public sendMessage(message: string) {
        this._signalR.connect().then((connection) => {
            connection.invoke('Send', message);
        });
    }
}