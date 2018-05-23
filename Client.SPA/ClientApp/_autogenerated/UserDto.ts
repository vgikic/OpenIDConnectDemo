
//----------------------------------------------
//  DTO interface and model
//----------------------------------------------

export interface IUserDto  {
    Id: string;
    Token: string;
    TokenRenewalTime: number;
    TokenExpirationTime: number;
    Roles: string[];
    GivenName: string;
    FamilyName: string;
    SubscriptionLevel: string;
}

export class UserDto  implements IUserDto{ 
    public Id: string;
    public Token: string;
    public TokenRenewalTime: number;
    public TokenExpirationTime: number;
    public Roles: string[];
    public GivenName: string;
    public FamilyName: string;
    public SubscriptionLevel: string;

    constructor(id : string = null,  token : string = null,  tokenRenewalTime : number = null,  tokenExpirationTime : number = null,  roles : string[] = null,  givenName : string = null,  familyName : string = null,  subscriptionLevel : string = null ) {
        
        this.Id = id;
        this.Token = token;
        this.TokenRenewalTime = tokenRenewalTime;
        this.TokenExpirationTime = tokenExpirationTime;
        this.Roles = roles;
        this.GivenName = givenName;
        this.FamilyName = familyName;
        this.SubscriptionLevel = subscriptionLevel;
    }
}


