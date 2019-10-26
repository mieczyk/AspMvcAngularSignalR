import { Component } from '@angular/core';
import { SignalR, BroadcastEventListener } from 'ng2-signalr';

import { ReceivedMessage } from './models/message.model';

@Component({
  selector: 'chat-window',
  templateUrl: './chat-window.component.html',
  styles: []
})
export class ChatWindow {
    private messages: ReceivedMessage[];

    constructor(private _signalR: SignalR) { }

    ngOnInit() {
        this.messages = [];
        this._signalR.connect().then((connection) => {
            let onMessagesReceived = new BroadcastEventListener<string>('onMessagesReceived');

            connection.listen(onMessagesReceived);
            onMessagesReceived.subscribe((messagesJson: string) => {
                this.updateChatWindow(messagesJson);
            });
        });
    }

    private updateChatWindow(messagesJson: string) {
        if (messagesJson) {
            this.messages = []
            for (let msg of messagesJson) {
                this.messages.push(new ReceivedMessage(msg['Sender'], msg['Body']));
            }
        }
    }
}
