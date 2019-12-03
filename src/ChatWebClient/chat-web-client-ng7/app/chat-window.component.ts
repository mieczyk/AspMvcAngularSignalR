import { Component } from '@angular/core';
import { SignalR, BroadcastEventListener } from 'ng2-signalr';

@Component({
  selector: 'chat-window',
  templateUrl: './chat-window.component.html',

})
export class ChatWindow {
    messages: string[];

    constructor(private _signalR: SignalR) { }

    ngOnInit() {
        this.messages = [];

        this._signalR.connect().then((connection) => {
            let onMessageReceived = new BroadcastEventListener<string>('onMessageReceived');

            connection.listen(onMessageReceived);

            onMessageReceived.subscribe((message: string) => {
                this.messages.push(message);
            });
        });
    }
}
