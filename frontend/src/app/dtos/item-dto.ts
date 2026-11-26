import { CharacterType } from '../enums/character-type';

export interface ItemDto {
    id: number;
    character: string;
    characterType: CharacterType;
    meaning: string | null;
    jlptLevel: number | null;
    beltId: number;
}
