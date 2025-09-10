export interface UserRoleMod{
    id : number,
    active: boolean

    userId : number,
    userName : string,

    roleId: number,
    roleName: string,
}

export interface UserRoleOptionsMod{
    id : number,
    active: boolean

    userId : number,
    roleId : number,
}