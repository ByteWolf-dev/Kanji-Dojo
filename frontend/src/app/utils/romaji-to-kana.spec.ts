import { romajiToKana, toHiragana, toKatakana } from './romaji-to-kana';

describe('romajiToKana', () => {

    it('converts simple syllables', () => {
        expect(romajiToKana('sakura')).toBe('さくら');
        expect(romajiToKana('shi')).toBe('し');
        expect(romajiToKana('tsuchi')).toBe('つち');
        expect(romajiToKana('fuji')).toBe('ふじ');
    });

    it('supports alternative spellings', () => {
        expect(romajiToKana('si')).toBe('し');
        expect(romajiToKana('tu')).toBe('つ');
        expect(romajiToKana('hu')).toBe('ふ');
        expect(romajiToKana('zi')).toBe('じ');
    });

    it('converts combined syllables', () => {
        expect(romajiToKana('kyou')).toBe('きょう');
        expect(romajiToKana('jya')).toBe('じゃ');
        expect(romajiToKana('ja')).toBe('じゃ');
        expect(romajiToKana('ocha')).toBe('おちゃ');
        expect(romajiToKana('ryokou')).toBe('りょこう');
    });

    it('ignores upper case', () => {
        expect(romajiToKana('KaNa')).toBe('かな');
    });

    describe('n', () => {
        it('becomes ん before a consonant', () => {
            expect(romajiToKana('kanji')).toBe('かんじ');
            expect(romajiToKana('sensei')).toBe('せんせい');
        });

        it('becomes ん when typed twice', () => {
            expect(romajiToKana('kanna', true)).toBe('かんな');
            expect(romajiToKana('hon', true)).toBe('ほん');
            expect(romajiToKana('honn', true)).toBe('ほん');
            expect(romajiToKana('nnk')).toBe('んk');
        });

        it('splits nn before a vowel into ん and a syllable', () => {
            expect(romajiToKana('konnichiha')).toBe('こんにちは');
        });

        it('is separated by an apostrophe', () => {
            expect(romajiToKana("kin'en", true)).toBe('きんえん');
            expect(romajiToKana('kinen', true)).toBe('きねん');
        });

        it('waits for the next key at the end while typing', () => {
            expect(romajiToKana('hon')).toBe('ほn');
            expect(romajiToKana('honn')).toBe('ほnn');
            expect(romajiToKana('ほna')).toBe('ほな');
        });

        it('becomes ん at the end when final', () => {
            expect(romajiToKana('n', true)).toBe('ん');
            expect(romajiToKana('ほn', true)).toBe('ほん');
        });

        it('does not take the n from nya', () => {
            expect(romajiToKana('konnyaku')).toBe('こんにゃく');
        });
    });

    describe('small tsu', () => {
        it('is created from double consonants', () => {
            expect(romajiToKana('kitte')).toBe('きって');
            expect(romajiToKana('zasshi')).toBe('ざっし');
            expect(romajiToKana('gakkou')).toBe('がっこう');
        });

        it('is created from tch', () => {
            expect(romajiToKana('matcha')).toBe('まっちゃ');
        });

        it('can be typed directly', () => {
            expect(romajiToKana('xtsu')).toBe('っ');
        });
    });

    describe('long vowels', () => {
        it('keeps vowels as written in hiragana', () => {
            expect(romajiToKana('toukyou')).toBe('とうきょう');
            expect(romajiToKana('ookii')).toBe('おおきい');
            expect(romajiToKana('oneesan', true)).toBe('おねえさん');
        });

        it('converts a dash to the long vowel mark', () => {
            expect(romajiToKana('ko-hi-', false, true)).toBe('コーヒー');
        });
    });

    it('keeps kana that is already converted', () => {
        expect(romajiToKana('さくらm')).toBe('さくらm');
        expect(romajiToKana('さくらmo')).toBe('さくらも');
    });

    it('keeps unfinished syllables while typing', () => {
        expect(romajiToKana('ky')).toBe('ky');
        expect(romajiToKana('sh')).toBe('sh');
    });

    it('converts to katakana', () => {
        expect(romajiToKana('kamera', true, true)).toBe('カメラ');
        expect(romajiToKana('ka', false, true)).toBe('カ');
    });
});

describe('toHiragana / toKatakana', () => {
    it('converts between the two', () => {
        expect(toHiragana('スイ')).toBe('すい');
        expect(toKatakana('すい')).toBe('スイ');
        expect(toHiragana('みず')).toBe('みず');
    });
});
