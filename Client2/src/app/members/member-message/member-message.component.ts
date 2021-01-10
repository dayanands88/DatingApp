
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-message',
  templateUrl: './member-message.component.html',
  styleUrls: ['./member-message.component.css']
})
export class MemberMessageComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() username: string;
  messages: Message[];
  messageContent: string;

  constructor(public messageservice: MessageService) { }

  ngOnInit(): void {
    this.loadMessage();
  }
  loadMessage(){
    this.messageservice.getMessageThread(this.username).subscribe(messages => {
      this.messages = messages;
    })
  }

  sendMessage(){
    this.messageservice.sendMessage(this.username,this.messageContent).then(() => {
      // this.messages.push(message);
      this.messageForm.reset();
    })
  }

}
