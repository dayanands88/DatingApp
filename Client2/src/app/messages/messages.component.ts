import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MembersService } from '../_services/members.service';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
  styles: []
})
export class MessagesComponent implements OnInit {
  messages: Message[] =[];
  pagination: Pagination;
  container = 'Inbox';
  pageNumber = 1;
  pageSize = 5;
  constructor(private messageservice: MessageService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    this.messageservice.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(
      response => {
        this.messages = response.result;
        this.pagination = response.pagination;
      }
    )
  }
  deletemessage(id: number){
    this.messageservice.deleteMessage(id).subscribe(() => {
      this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
    })
  }
  pageChanged(event: any)
  {
    this.pageNumber = event.page;
    this.loadMessages();
  }

}
