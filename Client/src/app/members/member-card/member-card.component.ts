import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  PhotoUrl? : string;
  KnownAs ? :string;
  City?:string;
  Mem? :any;

  @Input()  members?  : Member ;

  constructor() { }

  ngOnInit(): void {

    this.PhotoUrl = this.members?.photoUrl;
    this.KnownAs = this.members?.knownAs;
    this.City=this.members?.city;
    
   

  }

}
