import { Photo } from "./Photo";

export interface Member {
    id: number;
    userName : string;
    photoUrl : string;
    age : number;
    knownAs : string;
    created : Date;
    lastActive : string;
    gender : string;
    introduction: string;
    lookingFor : string;
    intersts: string;
    city : string;
    country : string;
    photos : Photo[];
  }