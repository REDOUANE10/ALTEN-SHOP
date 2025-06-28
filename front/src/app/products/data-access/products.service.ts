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
    return this.http.get<Product[]>(`${this.path}GetAllProduct`).pipe(
      tap(products => this._products.set(products)),
      catchError(error => {
        console.error("Erreur lors de la récupération des produits :", error);
        return of([]); // retourne un tableau vide si erreur
      })
    );
  }

  public create(product: Product): Observable<boolean> {
    return this.http.post<boolean>(`${this.path}CreerUnProduit`, product).pipe(
      tap(success => {
        if (success) {
          this._products.update(products => [product, ...products]);
        }
      }),
      catchError(error => {
        console.error("Erreur lors de la création du produit :", error);
        return of(false);
      })
    );
  }

  public update(product: Product): Observable<boolean> {
    return this.http.patch<boolean>(`${this.path}UpdateProduct/${product.id}`, product).pipe(
      tap(success => {
        if (success) {
          this._products.update(products =>
            products.map(p => p.id === product.id ? product : p)
          );
        }
      }),
      catchError(error => {
        console.error("Erreur lors de la mise à jour du produit :", error);
        return of(false);
      })
    );
  }

  public delete(productId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.path}DeleteProduct/${productId}`).pipe(
      tap(success => {
        if (success) {
          this._products.update(products =>
            products.filter(product => product.id !== productId)
          );
        }
      }),
      catchError(error => {
        console.error("Erreur lors de la suppression du produit :", error);
        return of(false);
      })
    );
  }
}
