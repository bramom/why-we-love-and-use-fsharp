import { setType } from "fable-core/Symbol";
import _Symbol from "fable-core/Symbol";
import { createAtom, compareRecords, equalsRecords } from "fable-core/Util";
export class Complex {
  constructor(r, i) {
    this.r = r;
    this.i = i;
  }

  [_Symbol.reflection]() {
    return {
      type: "Program.Complex",
      interfaces: ["FSharpRecord", "System.IEquatable", "System.IComparable"],
      properties: {
        r: "number",
        i: "number"
      }
    };
  }

  Equals(other) {
    return equalsRecords(this, other);
  }

  CompareTo(other) {
    return compareRecords(this, other) | 0;
  }

}
setType("Program.Complex", Complex);
export class Color {
  constructor(r, g, b, a) {
    this.r = r | 0;
    this.g = g | 0;
    this.b = b | 0;
    this.a = a | 0;
  }

  [_Symbol.reflection]() {
    return {
      type: "Program.Color",
      interfaces: ["FSharpRecord", "System.IEquatable", "System.IComparable"],
      properties: {
        r: "number",
        g: "number",
        b: "number",
        a: "number"
      }
    };
  }

  Equals(other) {
    return equalsRecords(this, other);
  }

  CompareTo(other) {
    return compareRecords(this, other) | 0;
  }

}
setType("Program.Color", Color);
export const maxIter = 255;
export const height = 1024;
export const width = 1024;
export let minX = createAtom(-2);
export let maxX = createAtom(2);
export let minY = createAtom(-1.5);
export let maxY = createAtom(3.5);
export let rectX = createAtom(0);
export let rectY = createAtom(0);
export let rectW = createAtom(0);
export let rectH = createAtom(0);
export function iteratePoint(s, p) {
  return new Complex(s.r + p.r * p.r - p.i * p.i, s.i + 2 * p.i * p.r);
}
export function getIterationCount(p) {
  let z = p;
  let i = 0;

  while (i < maxIter ? z.r * z.r + z.i * z.i < 4 : false) {
    z = iteratePoint(p, z);
    i = i + 1 | 0;
  }

  return i | 0;
}
export function getCoord(x, y) {
  const p = new Complex(x * (maxX() - minX()) / width + minX(), y * (maxY() - minY()) / height + minY());
  return p;
}
export function getCoordColor(x, y) {
  const p = getCoord(x, y);
  const i = getIterationCount(p) | 0;
  return new Color(~~(255 / (i % 5)), ~~(255 / (i % 3)), ~~(255 / (i % 7)), 255);
}
export function showSet() {
  const ctx = document.getElementsByTagName("canvas")[0].getContext("2d");
  const img = ctx.createImageData(width, height);

  for (let y = 0; y <= height - 1; y++) {
    for (let x = 0; x <= width - 1; x++) {
      const index = (x + y * width) * 4 | 0;
      const color = getCoordColor(x, y);
      img.data[index + 0] = color.r;
      img.data[index + 1] = color.g;
      img.data[index + 2] = color.b;
      img.data[index + 3] = color.a;
    }
  }

  ctx.putImageData(img, 0, 0);
  ctx.fillStyle = "rgba(200,0,0,0.5)";
  ctx.fillRect(rectX(), rectY(), rectW(), rectH());
}
document.addEventListener("mousedown", function (de) {
  rectX(de.clientX);
  rectY(de.clientY);
  rectW(0);
  rectH(0);
  showSet();
  return {};
});
document.addEventListener("mousemove", function (de_1) {
  if (de_1.buttons === 1) {
    rectW(de_1.clientX - rectX());
    rectH(de_1.clientY - rectY());
    showSet();
  }

  return {};
});
document.addEventListener("mouseup", function (de_2) {
  const p1 = getCoord(~~rectX(), ~~rectY());
  const p2 = getCoord(~~(rectX() + rectW()), ~~(rectY() + rectH()));
  minX(p1.r < p2.r ? p1.r : p2.r);
  maxX(p1.r > p2.r ? p1.r : p2.r);
  minY(p1.i < p2.i ? p1.i : p2.i);
  maxY(p1.i > p2.i ? p1.i : p2.i);
  rectX(0);
  rectY(0);
  rectW(0);
  rectH(0);
  showSet();
  return {};
});
showSet();