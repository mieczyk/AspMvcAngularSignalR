import { Component } from '@angular/core';

import { SignalRService } from './signalr.service';
import { SentMessage } from './models/message.model';

@Component({
    selector: 'chat-sender',
    templateUrl: './chat-sender.component.html',
    styles: []
})
export class ChatSender {
    constructor(private signalRService: SignalRService) {}

    public sendMessage(messageString: string) {
        let message = SentMessage.fromString(messageString);

        if (!message.isEmpty()) {
            this.signalRService.getOpenConnection().then((connection) => {
                connection.invoke('Send', message);
            });
        }
    }
}