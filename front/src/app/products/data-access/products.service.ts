import { Injectable, inject, signal } from "@angular/core";
import { Product } from "./product.model";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable, of, tap } from "rxjs";

@Injectable({
    providedIn: "root"
}) 
export class ProductsService {

    private readonly http = inject(HttpClient);
    private readonly path = "https://localhost:7154/";
  
    private readonly _products = signal<Product[]>([]);
    public readonly products = this._products.asReadonly();

    public get(): Observable<Product[]> {
        return this.http.get<Product[]>(this.path+"GetAllProduct").pipe(
          tap((products) => {
            this._products.set(products);  // Mise à jour du signal avec les produits reçus
          }),
          catchError((error) => {
            console.error("Erreur lors de la récupération des produits depuis mon API", error);
            return of([]); // En cas d'erreur, on retourne un tableau vide
          })
        );
    }

    public create(product: Product): Observable<boolean> {
        return this.http.post<boolean>(this.path+"CreerUnProduit", product).pipe(
            catchError(() => {
                return of(true);
            }),
            tap(() => this._products.update(products => [product, ...products])),
            catchError((error) => {
                console.error("Erreur lors de la création du produit", error);
                return of(false); // ou throwError(() => error) si tu veux gérer ça côté composant
              })
              
        );
    }

    public update(product: Product): Observable<boolean> {
        return this.http.patch<boolean>(`${this.path}/${product.id}`, product).pipe(
            catchError(() => {
                return of(true);
            }),
            tap(() => this._products.update(products => {
                return products.map(p => p.id === product.id ? product : p)
            })),
        );
    }

    // public delete(productId: number): Observable<boolean> {
    //     return this.http.delete<boolean>(`${this.path}/${productId}`).pipe(
    //         catchError(() => {
    //             return of(true);
    //         }),
    //         tap(() => this._products.update(products => products.filter(product => product.id !== productId))),
    //     );
    // }
}