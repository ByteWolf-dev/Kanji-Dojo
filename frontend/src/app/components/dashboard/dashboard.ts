import { Component, inject, OnInit, signal } from '@angular/core';
import { Card } from '../../card/card';
import { CardService } from '../../services/card';
import { CardDto } from '../../dtos/card-dto';

@Component({
    selector: 'app-dashboard',
    imports: [Card],
    templateUrl: './dashboard.html',
    styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

    private cardService = inject(CardService);

    cards = signal<CardDto[]>([]);

    ngOnInit(): void {
        this.loadCards();
    }

    loadCards(): void {
        this.cardService.getCardsDueOnOrBefore().subscribe({
            next: (cards) => {
                this.cards.set(cards);
            },
            error: (err) => {
                console.error('Failed to load cards', err);
            }
        });
    }
}