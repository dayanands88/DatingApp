import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { Member } from "../_models/member";
import { MembersService } from "../_services/members.service";

@Injectable({
    providedIn: 'root'
})
export class MemberDetailedResolver implements Resolve<Member>{
    constructor(private memberservice: MembersService){}
    resolve(route: ActivatedRouteSnapshot): Member | Observable<Member> {
       return this.memberservice.getMember(route.paramMap.get('username'));
    }

}
