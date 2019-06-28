import { Component } from '@angular/core';

import { Message } from './models/message.model';

@Component({
  selector: 'app-root',
  templateUrl: './chat-window.component.html',
  styles: []
})
export class ChatWindow {
    messages: Message[];

    ngOnInit() {
        this.messages = [
            new Message('Bolek', 'Hello world'),
            new Message('Someone', 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum'),
            new Message('Lolek', 'Hi Bolek!'),
        ];
    }
}
