
//----------------------------------------------
//  DTO interface and model
//----------------------------------------------

export interface IUserDto  {
    Token: string;
    TokenExpirationTime: number;
}

export class UserDto  implements IUserDto{ 
    public Token: string;
    public TokenExpirationTime: number;

    constructor(token : string = null,  tokenExpirationTime : number = null ) {
        
        this.Token = token;
        this.TokenExpirationTime = tokenExpirationTime;
    }
}


