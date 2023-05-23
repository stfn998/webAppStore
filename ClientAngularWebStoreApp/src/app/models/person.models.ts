export interface Person{ 
    id: number,
    firstName: string,
    lastName: string,
    userName: string,
    email: string,
    address: string,
    imageUrl: string,
    birth: string,
    personType: number,
    verification: string
}

export class Person {
    id!: number;
    firstName!: string;
    lastName!: string;
    userName!: string;
    email!: string;
    address!: string;
    imageUrl!: string;
    birth!: string;
    personType!: number;
    verification!: string;
}