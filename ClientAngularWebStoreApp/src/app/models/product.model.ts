export interface Product{ 
    id: number,
    sellerId: number,
    name: string,
    price: number,
    description: string,
    imageUrl: string,
    quantity: number
}

export class Product {
    id!: number;
    sellerId!: number;
    name!: string;
    price!: number;
    description!: string;
    imageUrl!: string;
    quantity!: number;
}