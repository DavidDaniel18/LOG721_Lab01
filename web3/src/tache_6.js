const { Web3 } = require('web3')
const fs = require('fs')
require('dotenv').config()

// Attention, il faut remplacer les valeurs selon votre config sur Ganache
const port = process.env.PORT || 7545
const contractJsonFilePath = process.env.CONTRACT_JSON_PATH
const contractAddress = process.env.CONTRACT_ADDRESS
const accountAddress = process.env.ACCOUNT_ADDRESS

const url = 'http://127.0.0.1:' + port;

const web3 = new Web3(url)
web3.defaultAccount = accountAddress;
web3.defaultNetworkId = 5777;

const contractJSON = JSON.parse(fs.readFileSync(contractJsonFilePath, 'utf-8'))
const studentContract = new web3.eth.Contract(contractJSON.abi, contractAddress)

console.log(`Address: ${url}`);
console.log(`Contract Json Path: ${process.env.CONTRACT_JSON_PATH}`);
console.log(`Contract Address: ${process.env.CONTRACT_ADDRESS}`);
console.log(`Account Address: ${process.env.ACCOUNT_ADDRESS}`);
console.log('')

async function setStudentNumber(studentNumber) {
  try {
    const gasEstimate = await studentContract.methods
      .setStudentNumber(studentNumber)
      .estimateGas({
        from: accountAddress,
        value: 5400000000000000,
      });

    const result = await studentContract.methods
      .setStudentNumber(studentNumber)
      .send({
        from: accountAddress,
        gas: gasEstimate,
        value: 5400000000000000,
      });

    console.log('Transaction hash:', result.transactionHash);
  } catch (error) {
    console.error('Error setting student number:', error);
  }
}

async function getStudentNumber() {
  try {
    const number = await studentContract.methods.getStudentNumber().call();

    console.log('Student Number:', number);
  } catch (error) {
    console.error('Error getting student number:', error);
  }
}

// Attention, vu que c'est du async, il se peut que getStudentNumber fini avant addStudentNumber.
// C'était juste pour tester le comportement. Selon le chargé, on va réutilisé ce code dans la partie 4
async function main() {
  console.log('Set student number...')
  await setStudentNumber(20)
  console.log('Get student number...')
  await getStudentNumber()
}

// Run everything
main();