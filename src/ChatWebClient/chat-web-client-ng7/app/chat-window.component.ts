import { Component } from '@angular/core';
import { SignalR, BroadcastEventListener } from 'ng2-signalr';

import { Message } from './models/message.model';

@Component({
  selector: 'app-root',
  templateUrl: './chat-window.component.html',
  styles: []
})
export class ChatWindow {
    private messages: Message[];

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
                this.messages.push(new Message(msg['Sender'], msg['Body']));
            }
        }
    }
}
